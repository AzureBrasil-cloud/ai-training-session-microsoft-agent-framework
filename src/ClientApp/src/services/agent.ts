import type { MessageResult } from '@/models/messageResult'
import { Role } from '@/models/messageResult'
import type { Thread } from '@/models/thread'
import axios, { AxiosError } from 'axios'

export interface CreateThreadRequest {
  feature: number;
}

export interface CreateRunRequest {
  feature: number;
  threadId: string;
  message: string;
  agentName: string;
  agentInstructions: string;
}

export interface RunResponse {
  role: number;
  content: string;
  usage?: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}

const agentService = {
  createThread: async (request: CreateThreadRequest): Promise<Thread> => {
    try {
      const response = await axios.post<Thread>('/api/agents/thread', request)
      return response.data
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  },

  listThreads: async (feature: number): Promise<Thread[]> => {
    try {
      const response = await axios.get<Thread[]>(`/api/agents/threads?Feature=${feature}`)
      return response.data
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  },

  getMessages: async (threadId: string): Promise<MessageResult[]> => {
    try {
      const response = await axios.get<{ messages: MessageResult[] }>(`/api/agents/threads/${threadId}/messages`)
      return response.data.messages
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  },

  run: async (request: CreateRunRequest): Promise<MessageResult> => {
    try {
      const response = await axios.post<RunResponse>('/api/agents/run', request)
      return {
        role: response.data.role as Role,
        content: response.data.content,
        usage: response.data.usage
      }
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  },
  runWorkflow: async (params: {
  feature: number;
  agentName: string;
  agentInstructions: string;
  threadId: string;
  message: string;
  agents: Array<{
    agentName: string;
    agentInstructions: string;
    feature: number;
  }>;
}): Promise<MessageResult> => {
  const response = await axios.post('api/agents/run-workflow', params);
  return response.data;
}
}

export default agentService


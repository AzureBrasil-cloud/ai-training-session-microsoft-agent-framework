import axios, { AxiosError } from 'axios'

export interface ChatCompletionRequest {
  instructions: string;
  message: string;
}

export interface ChatCompletionResult {
  content: string;
  usage?: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}

const aiInferenceService = {
  complete: async (request: ChatCompletionRequest): Promise<ChatCompletionResult> => {
    try {
      const response = await axios.post<ChatCompletionResult>('/api/ai-inference/complete', request)
      return response.data
    } catch (error: unknown) {
      const axiosError = error as AxiosError
      throw axiosError?.response?.data
    }
  }
}

export default aiInferenceService


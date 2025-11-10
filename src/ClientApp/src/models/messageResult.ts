export interface MessageResult {
  role: 'User' | 'Agent';
  content: string;
  usage?: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}


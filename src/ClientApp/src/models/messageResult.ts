export enum Role {
  User = 1,
  Agent = 2
}

export interface MessageResult {
  role: Role;
  content: string;
  usage?: {
    input?: number;
    output?: number;
    total?: number;
  } | null;
}


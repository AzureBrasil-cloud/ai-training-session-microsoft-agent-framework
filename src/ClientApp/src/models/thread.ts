export interface Thread {
  id: string;
  feature?: number;
  firstTruncatedMessage?: string | null;
}

export interface ThreadListResult {
  threadIds: string[];
}


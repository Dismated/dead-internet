import { Reply } from './comment.model';
export interface PostData {
  data: Post[];
}
export interface Post {
  comments: Reply[];
  createdAt: string;
  id: string;
  title: string;
}

export interface PromptNRepliesData {
  data: {
    prompt: Reply;
    replies: Reply[];
  }
}


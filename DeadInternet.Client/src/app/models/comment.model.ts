export interface Comment {
  data: {
    prompt: Reply
    replies: Reply[]
  }
}
export interface Reply {
  content: string;
  createdAt: string;
  id: string;
  parentCommentId: string;
  postId: string;
  replies: Reply[]
}

export interface CommentInput {
  content: string;
  parentCommentId: string;
}

import { Author } from "./author.model";

export class Blog {
  blogId: number;
  title: string;
  body: string;
  publishedDate: Date;
  authorId: number;
  author: Author;
}

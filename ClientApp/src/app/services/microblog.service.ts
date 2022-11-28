import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Author } from '../shared/models/author.model';
import { Observable } from 'rxjs';
import { Blog } from '../shared/models/blog.model';

@Injectable({
  providedIn: 'root'
})
export class MicroblogService {

  private baseUrl = 'http://localhost:5188/api/';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private httpClient: HttpClient) { }

  addAuthor(author: Author) {
    return this.httpClient.post<Author>(this.baseUrl + 'author/postauthor', author,this.httpOptions);
  }

  updateAuthor(author: Author): Observable<Author> {
    return this.httpClient.put<Author>(this.baseUrl + 'author/updateauthor/' + `${author.authorId}`, author);
  }

  getAuthors() {
    return this.httpClient.get(this.baseUrl + 'author/getauthors');
  }

  deleteAuthor(author: Author): Observable<Author> {
    return this.httpClient.delete<Author>(this.baseUrl + 'author/deleteauthor/' + author.authorId);
  }

  getBlogs(authorId: number) {
    if (authorId == undefined) {
      return this.httpClient.get(this.baseUrl + 'blog/getblogs');
    } else {
      return this.httpClient.get(this.baseUrl + 'blog/getblogs/' + authorId);
    }
  }

  getBlogsByAuthor(id: number) {
    return this.httpClient.get(this.baseUrl + 'blog/getblogs' + '?id=' + id);
  }

  addBlog(blog: Blog) {
    return this.httpClient.post<Blog>(this.baseUrl + 'blog/postblog', blog, this.httpOptions);
  }

  updateBlog(blog: Blog): Observable<Blog> {
    return this.httpClient.put<Blog>(this.baseUrl + 'blog/updateblog/' + `${blog.blogId}`, blog);
  }

  deleteBlog(blog: Blog): Observable<Blog> {
    return this.httpClient.delete<Blog>(this.baseUrl + 'blog/deleteblog/' + blog.blogId);
  }
}

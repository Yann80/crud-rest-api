import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { MicroblogService } from '../services/microblog.service';
import { Author } from '../shared/models/author.model';
import { Blog } from '../shared/models/blog.model';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.css']
})
export class BlogComponent implements OnInit {

  blogs: Blog[] = [];
  authors: Author[] = [];
  selectedBlog?: Blog;
  newBlog: Blog;
  blogForm: FormGroup;
  routeSub: Subscription;
  authorIdParam: any;
  result: string = '';

  constructor(private service: MicroblogService, private formBuilder: FormBuilder, private route: ActivatedRoute, private location: Location, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {

    this.blogForm = this.formBuilder.group({
      author: ['', [Validators.required]],
      title: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern("^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$")
      ])),
      body: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern("^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$")
      ])),
    });

    this.spinner.show();

    this.routeSub = this.route.params.subscribe(params => {
      this.authorIdParam = params['id'];
    });

    this.service.getAuthors()
      .subscribe(
        (result: any) => {
          this.authors = result;
          this.service.getBlogs(this.authorIdParam)
            .subscribe(
              (result: any) => {
                this.spinner.hide();
                this.blogs = result;
                this.blogs.forEach(blog => blog.author = this.authors.filter(a => a.authorId == blog.authorId)[0]);
              }
            );
        }
    );
  }

  onSubmit(form: FormGroup) {

    let author = this.authors.filter(a => a.authorId == form.value.author)[0];

    const newBlog: Blog = {
      blogId: form.value.blogId,
      title: form.value.title,
      body: form.value.body,
      authorId: author.authorId,
      publishedDate: new Date,
      author: author
    };

    if (this.selectedBlog == undefined) {
      this.service.addBlog(newBlog).subscribe(data => {
        data.author = author;
        this.blogs.push(data);
        this.blogForm.reset();
      });
    }
    else {
      newBlog.blogId =  this.selectedBlog.blogId
      this.service.updateBlog(newBlog).subscribe({
        next: (data) => {
          const index = this.blogs.findIndex(
            (blog) => blog.blogId === this.selectedBlog?.blogId
          );
          this.blogs[index].author = author;
          this.blogs[index].title = this.blogForm.get("title")?.value;
          this.blogs[index].body = this.blogForm.get("body")?.value;
        }
      });
    }
  }

  deleteBlog(blog: Blog) {
    if (confirm("Confirmer suppression")) {
      this.service.deleteBlog(blog).subscribe({
        next: (data) => {
          const index = this.blogs.findIndex(
            (blog) => blog.blogId === blog.blogId
          );
          this.blogs.splice(index, 1);
        }
      });
    }
  }

  editBlog(blog: Blog) {
    this.selectedBlog = blog;
    this.blogForm.get("author")?.patchValue(this.selectedBlog.author.authorId);
    this.blogForm.get("title")?.setValue(this.selectedBlog.title);
    this.blogForm.get("body")?.setValue(this.selectedBlog.body);
  }

  previousPage() {
    this.location.back();
  }

  get getControl() {
    return this.blogForm.controls;
  }
}

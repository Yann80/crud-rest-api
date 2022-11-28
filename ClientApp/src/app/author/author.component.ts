import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common'
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MicroblogService } from '../services/microblog.service';
import { Author } from '../shared/models/author.model';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit {

  authors: Author[] = [];
  selectedAuthor?: Author;
  newAuthor: Author;
  authorForm: FormGroup;

  constructor(private service: MicroblogService, private formBuilder: FormBuilder, private router: Router, private location: Location, private spinner: NgxSpinnerService) { }

  ngOnInit() {

    this.authorForm = this.formBuilder.group({
      firstName: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern("^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$")
      ])),
      lastName: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern("^[a-zA-Z0-9][a-zA-Z0-9.,'\-_ ]*[a-zA-Z0-9]$")
      ])),
      email: ['', [
        Validators.required, Validators.email]],
    });

    this.spinner.show();
    this.service.getAuthors()
      .subscribe(
        (result: any) => {
          this.authors = result;
          this.spinner.hide();
        }
      );
  }

  onSelect(author: Author): void {
    this.selectedAuthor = author;
  }

  onSubmit(form: FormGroup) {
    const objAuthor: Author = {
      authorId: form.value.authorId,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      email: form.value.email,
      creationDate: new Date
    };

    if (this.selectedAuthor == undefined) {
      this.service.addAuthor(objAuthor).subscribe(data => {
        this.authors.push(data);
        this.authorForm.reset();
      });

    }
    else {
      objAuthor.authorId = this.selectedAuthor.authorId;
      this.service.updateAuthor(objAuthor).subscribe({
        next: (data) => {
          const index = this.authors.findIndex(
            (author) => author.authorId === this.selectedAuthor?.authorId
          );
          this.authors[index].firstName = this.authorForm.get("firstName")?.value;
          this.authors[index].lastName = this.authorForm.get("lastName")?.value;
          this.authors[index].email = this.authorForm.get("email")?.value;
        }
      });
    }
  }

  deleteAuthor(author: Author) {
    if (confirm("Confirmer suppression")) {
      this.service.deleteAuthor(author).subscribe({
        next: (data) => {
          const index = this.authors.findIndex(
            (author) => author.authorId === author.authorId
          );
          this.authors.splice(index, 1);
        }
      });
    }
  }

  editAuthor(author:Author) {
    this.selectedAuthor = author;
    this.authorForm.get("firstName")?.setValue(this.selectedAuthor.firstName);
    this.authorForm.get("lastName")?.setValue(this.selectedAuthor.lastName);
    this.authorForm.get("email")?.setValue(this.selectedAuthor.email);
  }

  listPost(authorId: number) {
    this.router.navigate(['/list-billet/' + authorId]);
  }

  previousPage() {
    this.location.back();
  }

  get getControl() {
    return this.authorForm.controls;
  }
}

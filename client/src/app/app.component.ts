import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Market';
  product: any[];

  constructor(private http: HttpClient){}

  ngOnInit(): void {
    this.http.get('https://localhost5001/api/products').subscribe((response: any) => {
      console.log(response);
    }, error => {
      console.log(error);
    });
  }
}

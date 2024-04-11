import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, RouterModule],
  selector: 'web-app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  orders: any[] = [];
  year = new Date().getFullYear();

  constructor(http: HttpClient) {
    http.get<any>('/api/v1/customers/1/orders').subscribe((orders) => {
      this.orders = orders;
    });
  }
}

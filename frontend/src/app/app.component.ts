import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { LayoutComponent } from './common/components/layout/layout.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthApiService } from './common/services/api/auth-api.service';
import { MessageService } from 'primeng/api';
import { StoreService } from './common/services/store.service';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    LayoutComponent,
    HttpClientModule,
  ],
  providers: [AuthApiService, MessageService, StoreService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(private _storeService: StoreService) { }
}
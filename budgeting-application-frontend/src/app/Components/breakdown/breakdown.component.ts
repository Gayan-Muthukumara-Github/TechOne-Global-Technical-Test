import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Breakdown } from '../../Model/Breakdown';



@Component({
  selector: 'app-breakdown',
  standalone: true,
  imports: [],
  templateUrl: './breakdown.component.html',
  styleUrl: './breakdown.component.css'
})
export class BreakdownComponent implements OnInit {
  breakdownData: Breakdown[] = [];
  private apiUrl = 'https://localhost:7083/api/Transactions';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getBreakdown().subscribe((data) => {
      this.breakdownData = data;
    });
  }

  getBreakdown(): Observable<Breakdown[]> {
    return this.http.get<Breakdown[]>(`${this.apiUrl}/GetBreakdown`);
  }
}
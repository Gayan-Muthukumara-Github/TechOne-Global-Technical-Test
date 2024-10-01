import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Balance } from '../../Model/Balance';

@Component({
  selector: 'app-total-balance',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './total-balance.component.html',
  styleUrls: ['./total-balance.component.css'] 
})
export class TotalBalanceComponent implements OnInit {
  totalBalance: Balance | null = null;
  private apiUrl = 'https://localhost:7083/api/Transactions';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getTotalBalance().subscribe(
      (data) => {
        if (data.length > 0) {
          this.totalBalance = data[0]; 
        }
      },
      (error) => {
        console.error('Error fetching total balance', error); 
      }
    );
  }

  getTotalBalance(): Observable<Balance[]> {
    return this.http.get<Balance[]>(`${this.apiUrl}/GetTotalBalance`);
  }
}

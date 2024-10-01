import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Transaction } from '../Model/transaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  private apiUrl = 'https://localhost:7083/api/Transactions';
  
  constructor() { }
  http = inject(HttpClient);

  getAllTransactions() {
    return this.http.get<Transaction[]>(`${this.apiUrl}/GetAllTransaction`);
  }

  addTransaction(data: any) {
    return this.http.post(`${this.apiUrl}/AddTransaction`, data);
  }

  updateTransaction(transaction: Transaction) {
    return this.http.put(`${this.apiUrl}/UpdateTransaction/${transaction.transactionID}`, transaction);
  }

  deleteTransaction(id: number) {
    return this.http.delete(`${this.apiUrl}/DeleteTransaction/${id}`);
  }
}


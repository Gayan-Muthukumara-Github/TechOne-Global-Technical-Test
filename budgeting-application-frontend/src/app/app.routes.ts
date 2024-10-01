import { Routes } from '@angular/router';
import { TransactionComponent } from './Components/transaction/transaction.component';
import { TotalBalanceComponent } from './Components/total-balance/total-balance.component';
import { BreakdownComponent } from './Components/breakdown/breakdown.component';

export const routes: Routes = [
  { path: '', redirectTo: 'transaction', pathMatch: 'full' }, // Redirect root to transaction
  { path: 'transaction', component: TransactionComponent },  // Route for Transaction
  { path: 'total-balance', component: TotalBalanceComponent }, // Route for Total Balance (Optional)
  { path: 'breakdown', component: BreakdownComponent }, // Route for Breakdown (Optional)
];

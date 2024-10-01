import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Transaction } from '../../Model/transaction';
import { TransactionService } from '../../Service/transaction.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-transaction',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './transaction.component.html',
  styleUrl: './transaction.component.css'
})
export class TransactionComponent implements OnInit {
  @ViewChild('myModal') model: ElementRef | undefined;
  transactionList: Transaction[] = [];
  tranService = inject(TransactionService);
  transactionForm: FormGroup = new FormGroup({});

  constructor(private fb: FormBuilder) { }
  ngOnInit(): void {
    this.setFormState();
    this.getTransactions();
  }
  openModal() {
    const tranModal = document.getElementById('myModal');
    if (tranModal != null) {
      tranModal.style.display = 'block';
    }
  }

  closeModal() {
    this.setFormState();
    if (this.model != null) {
      this.model.nativeElement.style.display = 'none';
    }

  }
  getTransactions() {
    this.tranService.getAllTransactions().subscribe((res) => {

      this.transactionList = res;
    })
  }
  setFormState() {
    this.transactionForm = this.fb.group({

      transactionID: [0],
      amount: ['', [Validators.required]],
      currency: ['', [Validators.required]],
      currencytype: ['', [Validators.required]],
      category: ['', [Validators.required]],
      date: ['', [Validators.required]],
      type: ['', [Validators.required]],

    });
  }
  formValues: any;
  onSubmit() {
    debugger;
    console.log(this.transactionForm.value);
    if (this.transactionForm.invalid) { 
      alert('Please Fill All Fields');
      return;
    }
    if (this.transactionForm.value.transactionID == 0) {
      this.formValues = this.transactionForm.value;
      this.tranService.addTransaction(this.formValues).subscribe((res) => {

        alert('Transaction Added Successfully');
        this.getTransactions();
        this.transactionForm.reset();
        this.closeModal();

      });
    } else {
      this.formValues = this.transactionForm.value;
      this.tranService.updateTransaction(this.formValues).subscribe((res) => {

        alert('Transaction Updated Successfully');
        this.getTransactions();
        this.transactionForm.reset();
        this.closeModal();

      });
    }

  }


  OnEdit(Transaction: Transaction) {
    debugger;
    this.openModal();
    this.transactionForm.patchValue(Transaction);
  }
  onDelete(transaction: Transaction) {
    const isConfirm = confirm("Are you sure you want to delete this Transaction " + transaction.amount);
    if (isConfirm) {
      this.tranService.deleteTransaction(transaction.transactionID).subscribe((res) => {
        alert("Transaction Deleted Successfully");
        this.getTransactions();
      });
    }



  }
}
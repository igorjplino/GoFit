import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { forkJoin } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  constructor(private accountService: AccountService) { }

  init() {
    return forkJoin({
      user: this.accountService.getUserInfo()
    })
  }
}

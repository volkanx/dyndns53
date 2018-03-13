import { Injectable } from '@angular/core';

@Injectable()
export class LocalStorageService {

  constructor() { }

  setData(key: string, val: string): void {
    if (window.localStorage) {
      window.localStorage.setItem(key, val);
    }
  }

  getData(key: string): string | null {
    if (window.localStorage) {
      return window.localStorage.getItem(key);
    }
    return null;
  }
}

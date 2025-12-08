import { Component } from '@angular/core';
import { Footer } from '../footer/footer';
import { Register } from '../../features/register/register';
import { Login } from '../../features/login/login';
import { Translator } from '../translator/translator';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [Footer, Register, Login, Translator, RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home {
  protected isLogin: boolean = true;

  setRegisterMode() {
    this.isLogin = false;
  }

  setLoginMode() {
    this.isLogin = true;
  }
}

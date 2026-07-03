import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MainNavComponent } from './shared/main-nav/main-nav/main-nav';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    MainNavComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  protected readonly title =
    signal('HealthAxis');
}
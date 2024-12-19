import {Component, OnDestroy} from '@angular/core';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [
    RouterOutlet
  ],
  templateUrl: './todo.component.html',
  styleUrl: './todo.component.css'
})
export class TodoComponent implements OnDestroy {

  ngOnDestroy() {

  }
}

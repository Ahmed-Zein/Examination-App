import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-dashboard-item-card',
  standalone: true,
  imports: [],
  templateUrl: './dashboard-item-card.component.html',
  styleUrl: './dashboard-item-card.component.css'
})
export class DashboardItemCardComponent {

  @Input({required:true}) number!: number;
  @Input({required:true}) title!: string;
}

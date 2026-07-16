import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FreshnessService, FreshnessScore } from './freshness.service';

@Component({
  selector: 'app-root',
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private freshnessService = inject(FreshnessService);

  public scores = signal<FreshnessScore[]>([]);
  public loading = signal<boolean>(true);
  public errorMessage = signal<string>('');
  public syncingSource = signal<string>('');

  ngOnInit() {
    this.loadScores();
    // Poll freshness status every 5 seconds to simulate real-time updates
    setInterval(() => this.loadScores(false), 5000);
  }

  loadScores(showLoading = true) {
    if (showLoading) {
      this.loading.set(true);
    }
    this.freshnessService.getFreshnessScores().subscribe({
      next: (data) => {
        this.scores.set(data);
        this.loading.set(false);
        this.errorMessage.set('');
      },
      error: (err) => {
        console.error(err);
        this.errorMessage.set('Error al conectar con la API de Ingestión. Asegurate de levantar el backend.');
        this.loading.set(false);
      }
    });
  }

  syncSource(sourceName: string) {
    this.syncingSource.set(sourceName);
    this.freshnessService.syncSource(sourceName).subscribe({
      next: (res) => {
        this.loadScores(false);
        this.syncingSource.set('');
      },
      error: (err) => {
        console.error(err);
        this.syncingSource.set('');
      }
    });
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface FreshnessScore {
  sourceName: string;
  score: number;
  lastSyncTime: string;
  status: 'Green' | 'Yellow' | 'Red';
}

@Injectable({
  providedIn: 'root'
})
export class FreshnessService {
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5222'; // API Gateway / REST Endpoint

  getFreshnessScores(): Observable<FreshnessScore[]> {
    return this.http.get<FreshnessScore[]>(`${this.baseUrl}/api/freshness`);
  }

  syncSource(sourceName: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/api/sync/${sourceName}`, {});
  }
}

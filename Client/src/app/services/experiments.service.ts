import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Settings } from '../models/Settings.model';
import { environment } from 'src/environments/environment';
import { ExperimentStatistics } from '../models/ExperimentStatistics.model';

@Injectable({
  providedIn: 'root'
})
export class ExperimentsService {
  private options = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

  constructor(private http: HttpClient) {}

  launch(settings: Settings): Observable<ExperimentStatistics>{
    return this.http.post<ExperimentStatistics>(environment.urlPrefix + environment.experimentsUrl, settings);
  }
}

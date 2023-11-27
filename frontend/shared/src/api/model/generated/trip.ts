/**
 * Generated by orval v6.12.1 🍺
 * Do not edit manually.
 * Navtrack.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
 * OpenAPI spec version: 1.0
 */
import type { LocationModel } from './locationModel';

export interface Trip {
  locations?: LocationModel[] | null;
  startLocation?: LocationModel;
  endLocation?: LocationModel;
  readonly duration?: number;
  distance?: number;
  readonly maxSpeed?: number | null;
  readonly averageSpeed?: number | null;
  readonly averageAltitude?: number | null;
}

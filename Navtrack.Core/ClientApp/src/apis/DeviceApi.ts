import { apiUrl } from "framework/httpClient/HttpClientUtil";
import { ResponseModel } from "./types/ResponseModel";
import { HttpClient } from "framework/httpClient/HttpClient";
import { DeviceModel } from "./types/device/DeviceModel";
import { DeviceStatisticsResponseModel } from "./types/device/DeviceStatisticsResponseModel";

export const DeviceApi = {
  get: function (id: number): Promise<DeviceModel> {
    return HttpClient.get<DeviceModel>(apiUrl(`devices/${id}`));
  },

  getAll: function (): Promise<DeviceModel[]> {
    return HttpClient.get<DeviceModel[]>(apiUrl("devices"));
  },

  delete: function (id: number): Promise<ResponseModel> {
    return HttpClient.delete(apiUrl(`devices/${id}`));
  },

  getAvailableDevices: function (id?: number): Promise<DeviceModel[]> {
    const url = id ? `devices/available/${id}` : "devices/available";

    return HttpClient.get<DeviceModel[]>(apiUrl(url));
  },

  statistics: function (id: number): Promise<DeviceStatisticsResponseModel> {
    return HttpClient.get<DeviceStatisticsResponseModel>(apiUrl(`devices/${id}/statistics`));
  },

  put: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.put(apiUrl(`devices/${device.id}`), device);
  },

  add: function (device: DeviceModel): Promise<ResponseModel> {
    return HttpClient.post(apiUrl("devices"), device);
  }
};

import { DeviceTypeModel, DefaultDeviceTypeModel } from "./DeviceTypeModel";

export type DeviceModel = {
  id: number;
  deviceId: string;
  deviceType: DeviceTypeModel;
  locationsCount: number;
};

export const DefaultDeviceModel : DeviceModel = {
  id: 0,
  deviceId: "",
  deviceType: DefaultDeviceTypeModel,
  locationsCount: 0
};
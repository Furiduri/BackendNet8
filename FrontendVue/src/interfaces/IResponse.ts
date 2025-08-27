export interface IResponse<T = any> {
  error: number;
  msg: string;
  data: T;
}

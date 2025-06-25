export interface User {
  userId: number;
  username: string;
  role: 'User' | 'Admin';
  token: string;
}

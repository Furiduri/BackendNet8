export interface IUser {
	userId: number;
	username: string;
	email: string;
	available: boolean;
	roles?: string[];
	// Allow for PascalCase or camelCase from some backend responses
	UserId?: number;
	UserName?: string;
	userName?: string;
	Email?: string;
	Roles?: string[];
}

//String to string Base64
export function toBase64Unicode(str: string): string {    
    const base64 = btoa(str);
    return base64;
}

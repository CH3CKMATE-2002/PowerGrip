import { webcrypto } from "node:crypto";

const subtle = webcrypto.subtle; // Alias for easier use

export const generateAESKey = async (): Promise<webcrypto.CryptoKey> => {
    return await subtle.generateKey(
        { name: "AES-GCM", length: 256 },
        true,  // key is extractable (so we can export it)
        ["encrypt", "decrypt"]
    );
};

export const exportKey = async (key: webcrypto.CryptoKey): Promise<string> => {
    const exported = await subtle.exportKey("raw", key);
    return Buffer.from(exported).toString("base64");
};

export const importKey = async (base64Key: string): Promise<webcrypto.CryptoKey> => {
    const rawKey = Buffer.from(base64Key, "base64");
    return await subtle.importKey(
        "raw",
        rawKey,
        { name: "AES-GCM" },
        true,
        ["encrypt", "decrypt"]
    );
};

export const encryptAES = async (plaintext: string, key: webcrypto.CryptoKey): Promise<{ cipherText: string, iv: string }> => {
    const iv = webcrypto.getRandomValues(new Uint8Array(12)); // Generate a unique IV
    const encoder = new TextEncoder();
    
    const encrypted = await subtle.encrypt(
        { name: "AES-GCM", iv },
        key,
        encoder.encode(plaintext)
    );

    return {
        cipherText: Buffer.from(encrypted).toString("base64"), // Convert to Base64
        iv: Buffer.from(iv).toString("base64") // Store IV in Base64
    };
};

export const decryptAES = async(cipherText: string, ivBase64: string, key: webcrypto.CryptoKey): Promise<string> => {
    const iv = Buffer.from(ivBase64, "base64");
    const encryptedData = Buffer.from(cipherText, "base64");

    const decrypted = await subtle.decrypt(
        { name: "AES-GCM", iv },
        key,
        encryptedData
    );

    return new TextDecoder().decode(decrypted);
}
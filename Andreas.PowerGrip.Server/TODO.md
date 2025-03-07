# Server TODO-List!

- [ ] Key exchange!
    1. Server generates RSA Key-pair and gives public key to client.
    2. Client generates AES key, encrypts it with the public key.
    3. Server receives and decrypts the AES key and stores it somewhere safe.
    4. Client wants to update the system (for example), so it sends an encrypted system password with its own AES key.
    5. Server decrypts the password with the correct AES key.
    6. The decrypted password would be sent to the PAM helper program for further processing.
  - [X] Implement the encryption algorithms.
  - [ ] Encrypt the password.
  - [ ] Store the AES keys.
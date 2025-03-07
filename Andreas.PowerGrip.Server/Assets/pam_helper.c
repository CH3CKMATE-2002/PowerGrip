#include <security/pam_appl.h>  // PAM library for authentication
#include <stdio.h>              // Standard I/O functions
#include <stdlib.h>             // Memory allocation and exit functions
#include <string.h>             // String handling
#include <unistd.h>             // POSIX API (execvp, getpid, etc.)

// This struct will hold the user's response (password) for PAM authentication
struct pam_response *reply;

/**
 * PAM conversation function.
 * This function is called by PAM when it needs user input (e.g., password).
 * It provides the stored password to PAM automatically, without user input.
 */
int conversation(int num_msg, const struct pam_message **msg,
                 struct pam_response **resp, void *appdata_ptr) {
    // Allocate memory for the PAM response
    *resp = malloc(sizeof(struct pam_response));
    if (*resp == NULL) return PAM_BUF_ERR; // If memory allocation fails, return an error

    // Copy the provided password (from appdata_ptr) into the PAM response structure
    (*resp)->resp = strdup((char *)appdata_ptr);
    (*resp)->resp_retcode = 0;  // No error
    return PAM_SUCCESS;
}

int main(int argc, char *argv[]) {
    // Ensure the user provides at least a username (argv[1]) and optionally a command (argv[2])
    if (argc < 2) {  // Changed from <3 to <2
        fprintf(stderr, "Usage: %s <username> [command] [args...]\n", argv[0]);  // Updated usage
        return 1;
    }

    char *username = argv[1];  // Get the username from command-line arguments
    char password[1024];       // Buffer to store the password input

    // Read the password from standard input (stdin)
    if (fgets(password, sizeof(password), stdin) == NULL) {
        fprintf(stderr, "Failed to read password\n");
        return 1;
    }

    // Remove the newline character from the password
    password[strcspn(password, "\n")] = '\0';

    // Create a PAM handle
    pam_handle_t *pamh = NULL;

    // Define the PAM conversation structure
    struct pam_conv conv = {.conv = conversation, .appdata_ptr = password};

    // Start a new PAM session for authentication using the 'login' service
    int retval = pam_start("login", username, &conv, &pamh);
    if (retval != PAM_SUCCESS) {
        fprintf(stderr, "pam_start failed: %s\n", pam_strerror(pamh, retval));
        return 1;
    }

    // Perform authentication using PAM
    retval = pam_authenticate(pamh, 0);
    if (retval != PAM_SUCCESS) goto fail; // If authentication fails, jump to "fail" section

    // Check if the authenticated user is allowed to log in (account validation)
    retval = pam_acct_mgmt(pamh, 0);
    if (retval != PAM_SUCCESS) goto fail; // If account validation fails, jump to "fail" section

    // Close the PAM session after successful authentication
    pam_end(pamh, PAM_SUCCESS);

    // If additional arguments (a command) were provided, execute them
    if (argc >= 3) {  
        execvp(argv[2], &argv[2]); // Execute the given command with arguments
        perror("execvp failed");  // If execvp fails, print the error
        return 1;
    }

    return 0;  // Success if no command needed

fail:
    // Print error message if PAM authentication or account management failed
    fprintf(stderr, "Error: %s\n", pam_strerror(pamh, retval));
    pam_end(pamh, retval); // Close PAM session
    return 1;
}


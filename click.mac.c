#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <pthread.h>
#include <X11/Xlib.h>
#include <X11/Xutil.h>

#define CLICK_INTERVAL 100000 // Click interval in microseconds (100000 microseconds = 0.1 seconds)

// Structure to hold auto-clicker state
struct AutoClicker {
    int clicking;
    Display *display;
    pthread_t thread;
};

// Function declarations
void* click_loop(void* arg);
void start_clicking(struct AutoClicker *auto_clicker);
void stop_clicking(struct AutoClicker *auto_clicker);

// Click loop function
void* click_loop(void* arg) {
    struct AutoClicker *auto_clicker = (struct AutoClicker*)arg;
    while (auto_clicker->clicking) {
        XTestFakeButtonEvent(auto_clicker->display, 1, True, CurrentTime);
        XFlush(auto_clicker->display);
        usleep(CLICK_INTERVAL);
        XTestFakeButtonEvent(auto_clicker->display, 1, False, CurrentTime);
        XFlush(auto_clicker->display);
        usleep(CLICK_INTERVAL);
    }
    pthread_exit(NULL);
}

// Start auto-clicking
void start_clicking(struct AutoClicker *auto_clicker) {
    if (!auto_clicker->clicking) {
        auto_clicker->clicking = 1;
        pthread_create(&(auto_clicker->thread), NULL, click_loop, (void*)auto_clicker);
        printf("Auto-clicking started. Press 'q' to quit.\n");
    }
}

// Stop auto-clicking
void stop_clicking(struct AutoClicker *auto_clicker) {
    if (auto_clicker->clicking) {
        auto_clicker->clicking = 0;
        pthread_join(auto_clicker->thread, NULL);
        printf("Auto-clicking stopped.\n");
    }
}

int main() {
    struct AutoClicker auto_clicker = {0}; // Initialize auto-clicker

    // Open display
    auto_clicker.display = XOpenDisplay(NULL);
    if (auto_clicker.display == NULL) {
        fprintf(stderr, "Unable to open display.\n");
        return 1;
    }

    char choice;
    while (1) {
        printf("Press 's' to start auto-clicking, 'q' to quit: ");
        scanf(" %c", &choice);

        if (choice == 's') {
            start_clicking(&auto_clicker);
        } else if (choice == 'q') {
            stop_clicking(&auto_clicker);
            break;
        } else {
            printf("Invalid choice. Please try again.\n");
        }
    }

    // Close display
    XCloseDisplay(auto_clicker.display);
    return 0;
}

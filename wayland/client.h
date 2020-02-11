/* RilouwOS 0.1.0
 * Copyright (c) 2020 Jerome Martin
 * Released under the terms of the GNU AGPL version 3
 * http://rilouw.eu/projects/rilouwos
 */

#ifndef RILOUWSHELL_WAYLAND_CLIENT_H
#define RILOUWSHELL_WAYLAND_CLIENT_H

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <wayland-client.h>
#include <wayland-client-protocol.h>
#include <linux/input-event-codes.h>
#include "wlr-layer-shell-unstable-v1-protocol.h"

struct rsh_client_state {
  struct wl_compositor *compositor;
  struct zwlr_layer_shell_v1 *layer_shell;
  struct wl_seat *seat;

  bool running;
};

static void noop() { /* Do nothing */ }

static void
pointer_handle_button(
  void *data,
  struct wl_pointer *pointer,
  uint32_t serial,
  uint32_t time,
  uint32_t button,
  uint32_t state
) {
  if (button == BTN_LEFT && state == WL_POINTER_BUTTON_STATE_PRESSED) {
    printf("Button pressed\n");
  }
}

static const struct wl_pointer_listener pointer_listener = {
  .enter = noop,
  .leave = noop,
  .motion = noop,
  .button = pointer_handle_button,
  .axis = noop,
};

static void
seat_handle_capabilities(
  void *data,
  struct wl_seat *seat,
  uint32_t capabilities
) {
  if (capabilities & WL_SEAT_CAPABILITY_POINTER) {
    struct wl_pointer *pointer = wl_seat_get_pointer(seat);
    wl_pointer_add_listener(pointer, &pointer_listener, seat);
  }
}

static const struct wl_seat_listener seat_listener = {
  .capabilities = seat_handle_capabilities,
};

static void
handle_global(
  void *data,
  struct wl_registry *registry,
  uint32_t name,
  const char *interface,
  uint32_t version
) {
  struct rsh_client_state *state = data;
  if (strcmp(interface, wl_seat_interface.name) == 0) {
    state->seat = wl_registry_bind(registry, name, &wl_seat_interface, 1);
    wl_seat_add_listener(state->seat, &seat_listener, NULL);
    return;
  }

  if (strcmp(interface, zwlr_layer_shell_v1_interface.name) == 0) {
    state->layer_shell = wl_registry_bind(
      registry,
      name,
      &zwlr_layer_shell_v1_interface,
      zwlr_layer_shell_v1_interface.version
    );
    return;
  }

  if (strcmp(interface, wl_compositor_interface.name) == 0) {
    state->compositor = wl_registry_bind(registry, name, &wl_compositor_interface, 1);
    return;
  }
}

static void
handle_global_remove(
  void *data,
  struct wl_registry *registry,
  uint32_t name
) {
  // Do nothing
}

static const struct wl_registry_listener registry_listener = {
  .global = handle_global,
  .global_remove = handle_global_remove,
};

int
rsh_client_start() {
  struct rsh_client_state state = {
    .running = true,
  };

  struct wl_display *display = wl_display_connect(NULL);
  if (display == NULL) {
    fprintf(stderr, "Error: failed to create display\n");
    return EXIT_FAILURE;
  }

  struct wl_registry *registry = wl_display_get_registry(display);
  wl_registry_add_listener(registry, &registry_listener, &state);
  wl_display_roundtrip(display);

  if (!state.compositor || !state.layer_shell) {
    fprintf(stderr, "Error: missing required Wayland interfaces\n");
    return EXIT_FAILURE;
  }

  printf("Starting program loop\n");

  while (wl_display_dispatch(display) != -1 && state.running) {
    // Do nothing for now
  }

  return EXIT_SUCCESS;
}

#endif

/* RilouwOS 0.1.0
 * Copyright (c) 2020 Jerome Martin
 * Released under the terms of the GNU AGPL version 3
 * http://rilouw.eu/projects/rilouwos
 */

#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <wayland-client.h>
#include <linux/input-event-codes.h>
#include "wlr-layer-shell-unstable-v1-protocol.h"
#include "surface.h"

static void noop() { /* Do nothing */ }

static void
pointer_handle_button(
  void *data,
  struct wl_pointer *pointer,
  uint32_t serial,
  uint32_t time,
  uint32_t button,
  uint32_t btn_state
) {
  struct rsh_client_state *state = data;
  if (button == BTN_LEFT && btn_state == WL_POINTER_BUTTON_STATE_PRESSED) {
    printf("Button pressed\n");
    state->running = false;
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
  printf("Handling seat capabilities\n");
  struct rsh_client_state *state = data;
  state->seat = seat;
  if (capabilities & WL_SEAT_CAPABILITY_POINTER) {
    struct wl_pointer *pointer = wl_seat_get_pointer(seat);
    wl_pointer_add_listener(pointer, &pointer_listener, state);
  }
}

static const struct wl_seat_listener seat_listener = {
  .capabilities = seat_handle_capabilities,
};

static void
global_registry_handle(
  void *data,
  struct wl_registry *registry,
  uint32_t id,
  const char *interface,
  uint32_t version
) {
  struct rsh_client_state *state = data;
  printf("Receiving interface %s - ", interface);

  if (strcmp(interface, wl_seat_interface.name) == 0) {
    state->seat = wl_registry_bind(registry, id, &wl_seat_interface, 1);
    wl_seat_add_listener(state->seat, &seat_listener, state);
    printf("HANDLED\n");
    return;
  }

  if (strcmp(interface, zwlr_layer_shell_v1_interface.name) == 0) {
    state->shell = wl_registry_bind(
      registry,
      id,
      &zwlr_layer_shell_v1_interface,
      zwlr_layer_shell_v1_interface.version
    );
    printf("HANDLED\n");
    return;
  }

  if (strcmp(interface, wl_compositor_interface.name) == 0) {
    state->compositor = wl_registry_bind(registry, id, &wl_compositor_interface, 1);
    printf("HANDLED\n");
    return;
  }

  if (strcmp(interface, wl_shm_interface.name) == 0) {
    state->shm = wl_registry_bind(registry, id, &wl_shm_interface, 1);
    printf("HANDLED\n");
    return;
  }

  printf("NO\n");
}

static void
global_registry_remove(
  void *data,
  struct wl_registry *registry,
  uint32_t id
) {
  printf("Global remove\n");
  // Do nothing
}

static const struct wl_registry_listener registry_listener = {
  .global = global_registry_handle,
  .global_remove = global_registry_remove,
};

int
rsh_client_start() {
  struct rsh_client_state state = {
    .running = true,
  };

  struct wl_display *display = wl_display_connect(NULL);
  if (display == NULL) {
    fprintf(stderr, "Failed to create display\n");
    return EXIT_FAILURE;
  }

  struct wl_registry *registry = wl_display_get_registry(display);
  wl_registry_add_listener(registry, &registry_listener, &state);
  wl_display_roundtrip(display);

  if (!state.compositor || !state.shell) {
    fprintf(stderr, "Missing required Wayland interfaces\n");
    return EXIT_FAILURE;
  }

  rsh_init_surface(&state);

  printf("Starting program loop\n");

  while (state.running && wl_display_dispatch(display) != -1) {
    // Do nothing for now
  }

  wl_display_disconnect(display);
  printf("Disconnected from server\n");
  return EXIT_SUCCESS;
}

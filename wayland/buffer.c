/* RilouwOS 0.1.0
 * Copyright (c) 2020 Jerome Martin
 * Released under the terms of the GNU AGPL version 3
 * http://rilouw.eu/projects/rilouwos
 */

#include <stdlib.h>
#include <sys/mman.h>
#include <unistd.h>
#include <wayland-client.h>
#include "client.h"
#include "share.h"

void
rsh_init_buffer(struct rsh_client_state *state) {
  struct wl_shm_pool *pool;
  int stride = state->width * 4; // 4 bytes per pixel
  int size = stride * state->height;
  int fd;

  fd = rsh_create_shared_file(size);
  if (fd < 0) {
    fprintf(stderr, "Failed to create buffer file of size %d at %m\n", size);
    exit(EXIT_FAILURE);
  }
  
  state->shm_data = mmap(NULL, size, PROT_READ | PROT_WRITE, MAP_SHARED, fd, 0);
  if (state->shm_data == MAP_FAILED) {
    fprintf(stderr, "mmap failed: %m\n");
    close(fd);
    exit(EXIT_FAILURE);
  }

  pool = wl_shm_create_pool(state->shm, fd, size);
  state->buffer = wl_shm_pool_create_buffer(
    pool,
    0,
    state->width,
    state->height,
    stride,   
    WL_SHM_FORMAT_XRGB8888
  );
  wl_shm_pool_destroy(pool);
}

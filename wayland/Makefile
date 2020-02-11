# RilouwOS 0.1.0
# Copyright (c) 2020 Jerome Martin
# Released under the terms of the GNU AGPL version 3
# http://rilouw.eu/projects/rilouwos

wlr-layer-shell-protocol: wlr-layer-shell-unstable-v1.xml
	wayland-scanner client-header wlr-layer-shell-unstable-v1.xml wlr-layer-shell-unstable-v1-protocol.h
	wayland-scanner private-code wlr-layer-shell-unstable-v1.xml wlr-layer-shell-unstable-v1-protocol.c

xdg-shell-protocol: xdg-shell.xml
	wayland-scanner client-header xdg-shell.xml xdg-shell-protocol.h
	wayland-scanner private-code xdg-shell.xml xdg-shell-protocol.c

protocols: wlr-layer-shell-protocol xdg-shell-protocol

test: protocols
	gcc -g -Wall -I . -lwayland-client \
	wlr-layer-shell-unstable-v1-protocol.c \
	xdg-shell-protocol.c \
	test.c

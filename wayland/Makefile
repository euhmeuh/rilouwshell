# RilouwOS 0.1.0
# Copyright (c) 2020 Jerome Martin
# Released under the terms of the GNU AGPL version 3
# http://rilouw.eu/projects/rilouwos

.PHONY: clean
.DEFAULT_GOAL := lib

GCC=gcc -g -Wall -Werror -I . -L .

wlr-layer-shell-unstable-v1-protocol.c: wlr-layer-shell-unstable-v1.xml
	wayland-scanner private-code wlr-layer-shell-unstable-v1.xml wlr-layer-shell-unstable-v1-protocol.c

wlr-layer-shell-unstable-v1-protocol.h: wlr-layer-shell-unstable-v1.xml
	wayland-scanner client-header wlr-layer-shell-unstable-v1.xml wlr-layer-shell-unstable-v1-protocol.h

xdg-shell-protocol.c: xdg-shell.xml
	wayland-scanner private-code xdg-shell.xml xdg-shell-protocol.c

xdg-shell-protocol.h: xdg-shell.xml
	wayland-scanner client-header xdg-shell.xml xdg-shell-protocol.h

wlr-layer-shell-unstable-v1-protocol.o: \
	wlr-layer-shell-unstable-v1-protocol.c \
	wlr-layer-shell-unstable-v1-protocol.h
	$(GCC) -c -o $@ $<

xdg-shell-protocol.o: \
	xdg-shell-protocol.c \
	xdg-shell-protocol.h
	$(GCC) -c -o $@ $<

share.o: share.c share.h
	$(GCC) -c -o $@ $<

buffer.o: buffer.c buffer.h
	$(GCC) -c -o $@ $<

surface.o: surface.c surface.h
	$(GCC) -c -o $@ $<

client.o: client.c client.h
	$(GCC) -c -o $@ $<

rilouwshell-wayland-client.a: \
	wlr-layer-shell-unstable-v1-protocol.o \
	xdg-shell-protocol.o \
	share.o buffer.o surface.o client.o
	ar rcs $@ $^

lib: rilouwshell-wayland-client.a

test: lib
	$(GCC) -o test test.c -lwayland-client -l:rilouwshell-wayland-client.a

clean:
	rm -f test *.o *.a

.PHONY: run

run:
	gforth -- test/hello-app.fs -e ".s bye"

package io.github.ksrgtech.dape.commandline

import com.monovore.decline.Opts

final case class Options(
    runDir: String
)

object Options {

  val opts: Opts[Options] = {
    val runDir = Opts.option[String]("run-dir", help = "Run directory path")
    runDir.map(Options.apply)
  }
}

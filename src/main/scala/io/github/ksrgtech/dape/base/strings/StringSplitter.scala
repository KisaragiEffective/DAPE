package io.github.ksrgtech.dape.base.strings

object StringSplitter {

  def toSubstrings(haystack: String, needle: Char): Iterator[String] =
    new Iterator[String] {
      private var start: Int       = 0
      private var nextIndex: Int   = haystack.indexOf(needle)
      private var hasMore: Boolean = true

      override def hasNext: Boolean = hasMore

      override def next(): String = {
        if !hasMore then {
          throw new NoSuchElementException("No more elements")
        }

        if nextIndex == -1 then {
          hasMore = false
          haystack.substring(start)
        } else {
          val result = haystack.substring(start, nextIndex)
          start = nextIndex + 1
          nextIndex = haystack.indexOf(needle, start)
          result
        }
      }
    }
}

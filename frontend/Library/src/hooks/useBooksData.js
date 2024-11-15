import { useState, useEffect } from 'react';

const useBooksData = (page = 1, pageSize = 5) => {
  const [books, setBooks] = useState([]);
  const [genres, setGenres] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [totalBooks, setTotalBooks] = useState(0);

  const fetchGenres = async () => {
    const response = await fetch('https://localhost:7040/Genre', { credentials: 'include' });
    const data = await response.json();
    setGenres(data);
    return data;
  };

  const fetchAuthors = async () => {
    const response = await fetch('https://localhost:7040/Author', { credentials: 'include' });
    const data = await response.json();
    setAuthors(data);
    return data;
  };

  const fetchTotalBooksCount = async () => {
    const response = await fetch('https://localhost:7040/Books/count', { credentials: 'include' });
    const data = await response.json();
    setTotalBooks(data);
  };

  const fetchBooks = async (page, pageSize) => {
    setLoading(true);
    try {
      const response = await fetch(`https://localhost:7040/Books/getBypage/${page}/${pageSize}`, { credentials: 'include' });
      const data = await response.json();

      const genresData = await fetchGenres();
      const authorsData = await fetchAuthors();
      await fetchTotalBooksCount();

      const booksWithDetails = data.map((book) => {
        const genre = genresData.find((g) => g.id === book.genreId);
        const author = authorsData.find((a) => a.id === book.authorId);

        return {
          ...book,
          genreName: genre ? genre.genre : 'Unknown Genre',
          authorName: author ? `${author.name} ${author.surname}` : 'Unknown Author',
        };
      });

      setBooks(booksWithDetails);
    } catch (error) {
      console.error('Error fetching books:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchBooks(page, pageSize);
  }, [page, pageSize]);

  return { books, loading, totalBooks, fetchBooks };
};

export default useBooksData;

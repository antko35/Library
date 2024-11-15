import { useState, useEffect } from 'react';

const useFetchBook = (id) => {
  const [book, setBook] = useState(null);
  const [genres, setGenres] = useState([]);
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Функция для загрузки жанров
  const fetchGenres = async () => {
    try {
      const response = await fetch('https://localhost:7040/Genre', {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      setGenres(data);
      return data;
    } catch (error) {
      console.error('Ошибка при загрузке жанров:', error);
      setError('Ошибка при загрузке жанров');
      return [];
    }
  };

  // Функция для загрузки авторов
  const fetchAuthors = async () => {
    try {
      const response = await fetch('https://localhost:7040/Author', {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      setAuthors(data);
      return data;
    } catch (error) {
      console.error('Ошибка при загрузке авторов:', error);
      setError('Ошибка при загрузке авторов');
      return [];
    }
  };

  // Функция для загрузки книги
  const fetchBook = async () => {
    try {
      const response = await fetch(`https://localhost:7040/Books/getById/${id}`, {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Ошибка при загрузке информации о книге:', error);
      setError('Ошибка при загрузке информации о книге');
      return null;
    }
  };

  const loadBookData = async () => {
    setLoading(true);

    const [genresData, authorsData, bookData] = await Promise.all([
      fetchGenres(),
      fetchAuthors(),
      fetchBook(),
    ]);

    if (bookData) {
      const genre = genresData.find((g) => g.id === bookData.genreId);
      const author = authorsData.find((a) => a.id === bookData.authorId);
      setBook({ ...bookData, genre, author });
    }

    setLoading(false);
  };
  // Основной эффект для загрузки книги, жанров и авторов
  useEffect(() => {
    

    loadBookData();
  }, [id]);

  return { book, loading, error, setBook, loadBookData };
};

export default useFetchBook;

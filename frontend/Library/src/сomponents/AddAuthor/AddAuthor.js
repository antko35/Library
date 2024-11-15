import React, { useEffect, useState } from 'react';
import { Modal, Button, List, Input, Spin, DatePicker, message  } from 'antd';

const AddAuthor = ({ visible, onClose }) => {
  const [authors, setAuthors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [newAuthorName, setNewAuthorName] = useState('');
  const [newAuthorSurname, setNewAuthorSurname] = useState('');
  const [newAuthorCountry, setNewAuthorCountry] = useState('');
  const [newAuthorBirthDate, setNewAuthorBirthDate] = useState(null); 

  const fetchAuthors = async () => {
    setLoading(true);
    try {
      const response = await fetch('https://localhost:7040/Author', {
        method: 'GET',
        credentials: 'include',
      });
      const data = await response.json();
      setAuthors(data);
    } catch (error) {
      console.error('Error fetching authors:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (authorId) => {
    // Подтверждение перед удалением
    Modal.confirm({
      title: 'Are you shure?',
      content: 'This action cannot be undone.',
      okText: 'Yes',
      okType: 'danger',
      cancelText: 'No',
      onOk: async () => {
        try {
          await fetch(`https://localhost:7040/Author/${authorId}`, {
            method: 'DELETE',
            credentials: 'include',
          });
          fetchAuthors(); // Обновить список авторов после удаления
        } catch (error) {
          console.error('Error deleting author:', error);
        }
      },
    });
  };

  const handleAddAuthor = async () => {
    if (!newAuthorName || !newAuthorSurname || !newAuthorBirthDate || !newAuthorCountry) {
      message.error('Please, fill all fields.');
      return;
    }
    

    try {
      await fetch('https://localhost:7040/Author', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: newAuthorName, surname: newAuthorSurname, country: newAuthorCountry, birthDate: newAuthorBirthDate}),
        credentials: 'include',
      });
      fetchAuthors(); // Refresh authors after adding
      setNewAuthorName('');
      setNewAuthorSurname('');
      setNewAuthorCountry('');
      setNewAuthorBirthDate(null);
    } catch (error) {
      console.error('Error adding author:', error);
    }
  };

  useEffect(() => {
    if (visible) {
      fetchAuthors();
    }
  }, [visible]);

  return (
    <Modal
      title="Manage Authors"
      visible={visible}
      onCancel={onClose}
      footer={null}
    >
      {loading ? (
        <Spin tip="Loading authors..." />
      ) : (
        <>
          <Input
            placeholder="Author Name"
            value={newAuthorName}
            onChange={(e) => setNewAuthorName(e.target.value)}
            style={{ marginBottom: '10px' }}
          />
          <Input
            placeholder="Author Surname"
            value={newAuthorSurname}
            onChange={(e) => setNewAuthorSurname(e.target.value)}
            style={{ marginBottom: '10px' }}
          />
          <Input
            placeholder="Author country"
            value={newAuthorCountry}
            onChange={(e) => setNewAuthorCountry(e.target.value)}
            style={{ marginBottom: '10px' }}
          />
          <DatePicker
            placeholder="Дата рождения"
            value={newAuthorBirthDate}
            onChange={(date) => setNewAuthorBirthDate(date)} // Устанавливаем дату рождения
            style={{ marginBottom: '10px', width: '100%' }} // Задаем ширину для DatePicker
          />
          <Button type="primary" onClick={handleAddAuthor} style={{ marginBottom: '20px' }}>
            Add Author
          </Button>
         
          <List
            style={{ maxHeight: 200, overflowY: 'auto' }}
            bordered
            dataSource={authors}
            renderItem={(author) => (
              <List.Item
                actions={[
                  <Button type="link" danger onClick={() => handleDelete(author.id)}>
                    Delete
                  </Button>,
                ]}
              >
                {author.name} {author.surname}
              </List.Item>
            )}
          />
        </>
      )}
    </Modal>
  );
};

export default AddAuthor;

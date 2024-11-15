import React from 'react';
import {  Box, Text } from '@chakra-ui/react'; // or the appropriate component library
import { Flex,Button} from 'antd';
const AdminTools = ({ setIsModalVisible, setIsModalGenreVisible }) => {
  return (
    <div wrap style={{
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          marginBottom: '2%', 
        }}>
    <Box 
      border="1px solid #ccc" 
      borderRadius="8px" 
      padding="16px" 
      margin="16px 0"
      maxWidth="400px" 
      boxShadow="0 4px 8px rgba(0, 0, 0, 0.1)"
      textAlign="center" 
    >
      <Text fontSize="lg" fontWeight="bold" mb="8px" color="gray.600">
        Admin Tools
      </Text>
      <Flex gap="small" wrap="wrap" justify="center" align="center">
        <Button>Add book</Button>
        <Button onClick={() => setIsModalVisible(true)}>Manage authors</Button>
        <Button onClick={() => setIsModalGenreVisible(true)}>Manage genres</Button>
      </Flex>
    </Box>
    </div>
  );
};

export default AdminTools;

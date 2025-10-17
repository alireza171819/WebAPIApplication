import React from 'react';
import { makeStyles, shorthands, Text } from '@fluentui/react-components';
import ProductManagementPage from './Components/Product/ProductManagementPage.jsx';

const useStyles = makeStyles({
  app: {
    minHeight: '100vh',
    backgroundColor: '#f5f5f5',
    ...shorthands.padding('32px')
  },
  header: {
    marginBottom: '24px'
  }
});

const App = () => {
  const styles = useStyles();

  return (
    <div className={styles.app}>
      <Text as="h1" size={600} className={styles.header}>
        Product Management
      </Text>
      <ProductManagementPage />
    </div>
  );
};

export default App;
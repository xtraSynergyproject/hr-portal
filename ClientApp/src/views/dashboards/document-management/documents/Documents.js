import { Box } from '@mui/material'
import DocumentsView from './components/documents-view/DocumentsView'
import Header from './components/header/Header'
import Sidebar from './components/sidebar/Sidebar'

function Document() {
  return (
    <>
      <Header />
      <Box sx={{ display: 'flex', gap: '10px' }}>
        <Sidebar />
        <DocumentsView />
      </Box>
    </>
  )
}

export default Document

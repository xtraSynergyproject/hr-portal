import { Box } from '@mui/material'
import DocumentsViewHeader from './documents-view-header/DocumentsViewHeader'
import DocumentsViewTable from './documents-view-table/DocumentsViewTable'

const DocumentsView = () => {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
      <DocumentsViewHeader />
      <DocumentsViewTable />
    </Box>
  )
}

export default DocumentsView

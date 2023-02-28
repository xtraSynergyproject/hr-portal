import { Folder } from '@mui/icons-material'
import {
  Box,
  CircularProgress,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography
} from '@mui/material'
import axios from 'axios'
import { useEffect, useState } from 'react'
import DescriptionIcon from '@mui/icons-material/Description'

const Loader = () => {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center' }}>
      <CircularProgress />
    </Box>
  )
}

const DocumentsViewTable = () => {
  let [documents, setDocuments] = useState([])
  let [isLoading, setIsLoading] = useState(false)

  const fetchSourceFolders = async () => {
    setIsLoading(true)
    try {
      const response = await axios.get(
        'https://webapidev.aitalkx.com/dms/query/GetSourceFolders?userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR',
        {
          headers: {
            'Access-Control-Allow-Origin': '*'
          }
        }
      )
      setDocuments(response.data)
      setIsLoading(false)
    } catch (error) {
      console.log(error)
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchSourceFolders()
  }, [])

  const fetchChildFoldersAndDocuments = async (document, event) => {
    const { key } = document
    if (event.detail === 2) {
      setIsLoading(true)
      try {
        const response = await axios.get(
          `https://webapidev.aitalkx.com/dms/query/GetChildFoldersAndDocuments?key=${key}&userId=45bba746-3309-49b7-9c03-b5793369d73c&portalName=HR`,
          {
            headers: {
              'Access-Control-Allow-Origin': '*'
            }
          }
        )
        setDocuments(response.data)
        setIsLoading(false)
      } catch (error) {
        setIsLoading(false)
        console.log(error)
      }
    }
  }
  return (
    <Box sx={{ width: '800px' }}>
      <TableContainer sx={{ width: '100%' }} component={Paper}>
        <Table sx={{ cursor: 'default' }} aria-label='simple table'>
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell align='left'>Note No.</TableCell>
              <TableCell align='left'>Created By</TableCell>
              <TableCell align='left'>Created Date</TableCell>
              <TableCell align='left'>Workflow Status</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {isLoading ? (
              <TableRow>
                <TableCell>
                  <Loader />
                </TableCell>
              </TableRow>
            ) : documents.length ? (
              documents.map(document => {
                if (!document.Document) {
                  return (
                    <TableRow
                      onClick={event => fetchChildFoldersAndDocuments(document, event)}
                      key={document.key}
                      sx={{
                        '&:last-child td, &:last-child th': { border: 0 },
                        '&:hover': {
                          backgroundColor: '#e2e8f0'
                        }
                      }}
                    >
                      <TableCell component='th' scope='row'>
                        <Box sx={{ display: 'flex', gap: '10px' }}>
                          <Folder
                            sx={{
                              color: document.Workspace ? 'blue' : '#fde047'
                            }}
                          />
                          {document.title}
                        </Box>
                      </TableCell>
                      <TableCell align='left'>{document.NoteNo}</TableCell>
                      <TableCell align='left'>{document.CreatedBy}</TableCell>
                      <TableCell align='left'>{document.CreatedDate}</TableCell>
                      <TableCell align='left'>{document.WorkflowServiceStatus}</TableCell>
                    </TableRow>
                  )
                } else {
                  return (
                    <TableRow
                      key={document.key}
                      sx={{
                        '&:last-child td, &:last-child th': { border: 0 },
                        '&:hover': {
                          backgroundColor: '#e2e8f0'
                        }
                      }}
                    >
                      <TableCell component='th' scope='row'>
                        <Box sx={{ display: 'flex', gap: '10px' }}>
                          <DescriptionIcon sx={{ color: 'blue' }} />
                          {document.title}
                        </Box>
                      </TableCell>
                      <TableCell align='left'>{document.NoteNo}</TableCell>
                      <TableCell align='left'>{document.CreatedBy}</TableCell>
                      <TableCell align='left'>{document.CreatedDate}</TableCell>
                      <TableCell align='left'>{document.WorkflowServiceStatus}</TableCell>
                    </TableRow>
                  )
                }
              })
            ) : (
              <TableRow>
                <TableCell>
                  <Typography variant='h6'>No Record Found</Typography>
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  )
}

export default DocumentsViewTable
